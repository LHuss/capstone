#! /bin/sh

UNITY_DOWNLOAD_CACHE="$(pwd)/unity_download_cache"
BASE_URL="https://download.unity3d.com/download_unity"
HASH="a8557a619e24" #this is the 2017.4.33f version hash
UNITY_PACKAGE_OSX="MacEditorInstaller/Unity.pkg"
WINDOWS_TARGET_PACKAGE="MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-2017.4.33f1.pkg"


download() {
	package=$1
	url="$BASE_URL/$HASH/$package"
	file=`basename "$url"`

	if [ ! -e $UNITY_DOWNLOAD_CACHE/`basename "$url"` ] ; then
		echo "$file does not exist. Downloading from $url: "
		mkdir -p "$UNITY_DOWNLOAD_CACHE"
		curl -o $UNITY_DOWNLOAD_CACHE/`basename "$url"` "$url"
	else
		echo "$file Exists. Skipping download."
	fi
}


install() {
	package=$1
	download "$package"

	echo "Installing `basename "$package"`"
	sudo installer -dumplog -package $UNITY_DOWNLOAD_CACHE/`basename "$package"` -target /
}



echo "Contents of Unity Download Cache:"
ls $UNITY_DOWNLOAD_CACHE

echo "Installing Unity..."
install $UNITY_PACKAGE_OSX
install $WINDOWS_TARGET_PACKAGE
