username=iolab
machine=192.168.0.16
taskname=ProtoTest
resultOnWindows=/C:/Test/result.txt
resultLocal=~/result.txt

echo "Starting testing..."
ssh $username@$machine schtasks.exe /Run /TN $taskname

sleep 1

while true; do
  status=`ssh $username@$machine schtasks.exe /Query /TN $taskname | tail -n 1 | awk '{print $3}'`

  if [ "$status" = "Ready" ]; then
    echo "Finished!"
    break
  fi

  sleep 1
done

echo "Copy test result..."
scp $username@$machine:$resultOnWindows $resultLocal

total=`cat $resultLocal | wc -l`
pass=`grep Passed $resultLocal | wc -l`
fail=`grep Failed $resultLocal | wc -l`
notrun=`grep Inconclusive $resultLocal | wc -l`

echo "Ran $total cases, $pass passed, $fail failed, $notrun is inconclusive."
