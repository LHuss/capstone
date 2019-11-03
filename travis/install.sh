#! /bin/sh


UNITY_DOWNLOAD_CACHE="$(pwd)/unity_download_cache"
UNITY_OSX_PACKAGE_URL="https://download.unity3d.com/download_unity/9c8dbc3421cb/MacEditorInstaller/Unity.pkg" #this is the 2017.4.31f version
UNITY_WINDOWS_TARGET_PACKAGE_URL="https://download.unity3d.com/download_unity/9c8dbc3421cb/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.4.31f1.pkg"


download() {

	URL=$1
	FILE=`basename "$URL"`

	if [ ! -e $UNITY_DOWNLOAD_CACHE/`basename "$URL"` ] ; then
		echo "$FILE does not exist. Downloading from $URL: "
		mkdir -p "$UNITY_DOWNLOAD_CACHE"
		curl -o $UNITY_DOWNLOAD_CACHE/`basename "$URL"` "$URL"
	else
		echo "$FILE Exists. Skipping download."
	fi
}


install() {
	PACKAGE_URL=$1
	download $1

	echo "Installing `basename "$PACKAGE_URL"`"
	sudo installer -dumplog -package $UNITY_DOWNLOAD_CACHE/`basename "$PACKAGE_URL"` -target /
}



echo "Contents of Unity Download Cache:"
ls $UNITY_DOWNLOAD_CACHE

echo "Installing Unity..."
install $UNITY_OSX_PACKAGE_URL
install $UNITY_WINDOWS_TARGET_PACKAGE_URL
