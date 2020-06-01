from bs4 import BeautifulSoup
import os
from requests import get
from requests.exceptions import RequestException
from contextlib import closing
import urllib
import shutil
import psutil
from tqdm import tqdm

class TqdmUpTo(tqdm):
    """Provides `update_to(n)` which uses `tqdm.update(delta_n)`."""
    def update_to(self, b=1, bsize=1, tsize=None):
        """
        b  : int, optional
            Number of blocks transferred so far [default: 1].
        bsize  : int, optional
            Size of each block (in tqdm units) [default: 1].
        tsize  : int, optional
            Total size (in tqdm units). If [default: None] remains unchanged.
        """
        if tsize is not None:
            self.total = tsize
        self.update(b * bsize - self.n)  # will also set self.n = b * bsize

def simple_get(url):
    try:
        with closing(get(url, stream=True)) as resp:
            if is_good_response(resp):
                return resp.content
            else:
                return None

    except RequestException as e:
        log_error('Error during requests to {0} : {1}'.format(url, str(e)))
        return None


def is_good_response(resp):
    content_type = resp.headers['Content-Type'].lower()
    return (resp.status_code == 200
            and content_type is not None
            and content_type.find('html') > -1)


def log_error(e):
    print(e)

def Downloader7z():
    Link = "http://download.sourceforge.net/sevenzip/7za920.zip"
    Path = os.path.abspath('.')    
    ZipDir = os.path.join(Path, "7z")
    UnZip = os.path.join(Path, os.path.basename(Link)) 
    with TqdmUpTo(unit='B', unit_scale=True, miniters=1,
                  desc=Link.split('/')[-1]) as t:
        urllib.request.urlretrieve(Link, filename=UnZip,
                                   reporthook=t.update_to, data=None)
    shutil.unpack_archive(UnZip,ZipDir,'zip')
    os.remove(UnZip)

def LatestBuild():
    raw_html = simple_get("https://github.com/Revan654/staxrip/releases/latest")
    html = BeautifulSoup(raw_html, 'html5lib')
    for c, core in enumerate(html.select('li.py-1 > a > strong', limit=1)):
        indexCore=str(core.text)
        return(indexCore)

def LatestBuildTag():
    raw_html = simple_get("https://github.com/Revan654/staxrip/releases/latest")
    html = BeautifulSoup(raw_html, 'html5lib')
    for c, core in enumerate(html.select('.f1 > a', limit=1)):
        indexCore=str(core.text)
        TrimLink=indexCore.replace(" - x64", "")
        return(TrimLink)        
        
        
def DownloadStaxRip():        
    Link = "https://github.com/Revan654/staxrip/releases/download/{}/{}".format(LatestBuildTag(),LatestBuild())
    Path = os.path.abspath('.')
    StaxRip7z = os.path.join(Path, os.path.basename(Link))    
    with TqdmUpTo(unit='B', unit_scale=True, miniters=1,
                  desc=Link.split('/')[-1]) as t:
        urllib.request.urlretrieve(Link, filename=StaxRip7z,
                                   reporthook=t.update_to, data=None)
    ZipDir = os.path.join(Path, "7z")
    Exe = os.path.join(ZipDir, "7za.exe")
    File = os.path.join(Path, LatestBuild())      
    os.system('{} x -y "{}" -o"{}\"'.format(Exe, File, Path))
    os.remove(StaxRip7z)
    shutil.rmtree(ZipDir)
    
def get_version_number():
    Path = os.path.abspath('.')    
    StaxRip = os.path.join(Path,"StaxRip.exe")
    from win32api import GetFileVersionInfo, LOWORD, HIWORD
    info = GetFileVersionInfo (StaxRip, "\\")
    ms = info['FileVersionMS']
    ls = info['FileVersionLS']
    Version = "{}.{}.{}.{}".format(HIWORD (ms), LOWORD (ms), HIWORD (ls), LOWORD (ls))
    return(Version)    
    
def ProcessKill():
    Process = "StaxRip.exe"
    for proc in psutil.process_iter():    
        if proc.name() == Process:
            proc.kill()            
            
def StartStaxRip():
    Path = os.path.abspath('.')    
    StaxRip = os.path.join(Path,"StaxRip.exe") 
    os.system("{}".format(StaxRip))
    
def Main():
    Version = get_version_number()
    URLBuild = LatestBuildTag()
    if Version < URLBuild:
        print("Local Build: {}".format(Version))
        print("Release Build: {}".format(URLBuild))
        print("Downloading Latest Release")
        ProcessKill()
        print("Downloading 7z")
        Downloader7z()
        print("Downloading StaxRip")
        DownloadStaxRip()
        StartStaxRip()        
    else:
       print("Local Build: {}".format(Version))
       print("Release Build: {}".format(URLBuild))
       print("Your Already Running the Latest Release")       

if __name__ == '__main__':    
    Main()    