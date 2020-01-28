#! /bin/sh

PROJECT_PATH=$PWD/$UNITY_PROJECT_PATH
UNITY_BUILD_DIR=$PWD/Build
LOG_FILE=$UNITY_BUILD_DIR/unity-win.log


ERROR_CODE=0
echo "Items in project path ($PROJECT_PATH):"
ls "$PROJECT_PATH"


echo "Building project for Windows..."
mkdir $UNITY_BUILD_DIR
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile \
  -projectPath "$PROJECT_PATH" \
  -buildWindows64Player  "$UNITY_BUILD_DIR/win/ci-build.exe" \
  -quit \
  -force-free \
  | tee "$LOG_FILE"

if [ $? = 0 ] ; then
  echo "Building Windows exe completed successfully."
  ERROR_CODE=0
else
  echo "Building Windows exe failed. Exited with $?."
  ERROR_CODE=1
fi

#echo 'Build logs:'
#cat $LOG_FILE

echo "Finish code $ERROR_CODE"
exit $ERROR_CODE
