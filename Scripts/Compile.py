from distutils.core import setup
from distutils.extension import Extension
from Cython.Distutils import build_ext

ext_modules = [
    Extension("NVEnc",  ["NVEnc.py"]),
    Extension("QSVEnc",  ["QSVEnc.py"]),
    Extension("StaxRip",  ["StaxRip.py"]),
]

install_requires=[
   'requests',
   'tqdm',
   'beautifulsoup4',  
   'cython',
   'win32api'
   'psutil'
]

setup(
    name = 'Update',
    version = '0.2',
    cmdclass = {'build_ext': build_ext},
    ext_modules = ext_modules,
    platforms = 'Windows_x86_x64',
    requires = install_requires,
)
