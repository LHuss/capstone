#! /bin/sh

echo "Running Editor tests"

PROJECT_PATH=$PWD/$UNITY_PROJECT_PATH
ROOT=$PWD
ERROR_CODE=0

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
   -runEditorTests\
   -batchmode\
   -nographics\
   -projectPath "$PROJECT_PATH" \
   -editorTestsResultFile "../results.xml" \
   -stackTraceLogType Full \
   -logFile "$PROJECT_PATH/log.txt" \
   -force-free

if [ $? -ge 2 ] ; then
  echo "There was an error. Exited with $?."
  ERROR_CODE=1
else
  echo "All tests passed successfully."
  ERROR_CODE=0
fi

echo "Finish code $ERROR_CODE"
exit $ERROR_CODE