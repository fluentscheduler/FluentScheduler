Having Python 3 installed:

```
$ python3 --version
Python 3.6.7
```

And pip:

```
$ sudo apt install python3-pip
(...)
$ pip3 --version
pip 9.0.1 from /usr/lib/python3/dist-packages (python 3.6)
```

Install mkdocs:

```
$ pip3 install mkdocs
(...)
$ python3 -m mkdocs --version
__main__.py, version 1.0.4 from /home/talles/.local/lib/python3.6/site-packages/mkdocs (Python 3.6)
```

Then build the documentation running the following command on this folder:

```
$ python3 -m mkdocs build
INFO    -  Cleaning site directory
INFO    -  Building documentation to directory: /some/folder/FluentScheduler/Documentation/site
```