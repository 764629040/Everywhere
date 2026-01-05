import calendar
import datetime
import os
import pyotp
import subprocess
import sys
import time

def timecode(for_time: datetime.datetime, interval: int) -> int:
    if for_time.tzinfo:
        return int(calendar.timegm(for_time.utctimetuple())) % interval
    else:
        return int(time.mktime(for_time.timetuple())) % interval
    
def normalize_sha1(sha1: str) -> str:
    '''
    ONLY retain letters and numbers
    '''
    return ''.join(c for c in sha1 if c.isalnum())
    
def sign(figerprint: str, target_path: str):
    print(f"Signing {target_path}")
    this_script_pth = os.path.abspath(__file__)
    result = subprocess.run(
        [os.path.join(os.path.dirname(this_script_pth), 'signtool'), 'sign', '/sha1', figerprint, '/tr', 'http://time.certum.pl', '/td', 'sha256', '/fd', 'sha256', '/v', target_path],
        capture_output=True,
        text=True,
        encoding='utf-8'
    )

    if result.returncode != 0 or result.stdout.count("Error") > 0 or result.stderr.count("Error") > 0:
        raise RuntimeError(f"Sign tool failed: {result.stdout}\n{result.stderr}")
    
if __name__ == "__main__":
    # get username and otp token from environment variables
    
    username = os.getenv('SIGN_USERNAME')
    otp_token = os.getenv('SIGN_OTP_TOKEN')
    target_path = sys.argv[1]
    print(f"Start signing files in: {target_path}")

    totp = pyotp.TOTP(otp_token, digest='SHA256', issuer='Certum')

    while True:
        # Make sure to wait for the right time window
        # So that we have enough time to login
        if not (3 < timecode(datetime.datetime.now(), 30) < 13):
            time.sleep(0.1)
            continue

        break

    otp = totp.now()

    # run sign app with arguments, and read thumbprint from stdio
    creation_flags = 0x08000000 

    proc = subprocess.Popen(
        [r'C:\Program Files\Certum\SimplySign Desktop\SimplySignDesktop.exe', '/autologin', username, str(otp)], 
        stdout=subprocess.PIPE, 
        stderr=subprocess.STDOUT,
        text=True,
        encoding='utf-8',
        creationflags=creation_flags
    )

    figerprint: str = None
    while True:
        line = proc.stdout.readline()
        if not line:
            break

        figerprint = normalize_sha1(line.strip())
        break

    try:
        if not figerprint:
            raise RuntimeError("No fingerprints received from SimplySignDesktop.exe")

        # Run signtool on all exe in target path
        for file_name in os.listdir(target_path):
            if file_name.endswith('.exe'):
                sign(figerprint, os.path.join(target_path, file_name))
    finally:
        proc.terminate()