# versionup
A tool for upping version. It goes through files and replaces `oldVersion` with `newVersion` based on a text occurence. By default it processes csproj and props files.

## Usage
```
versionup [args] <oldVersion> <newVersion>
args list:
    -p <folder1>:      path
    -e <ext1,ext2...>: extensions, default extensions: csproj, props
    -?:                show this message
```
