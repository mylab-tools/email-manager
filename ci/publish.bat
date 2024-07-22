@echo off

IF [%1]==[] goto noparam

echo "Build image '%1' and 'latest'..."
docker build -f ./Dockerfile --build-arg PROJ=MyLab.EmailManager -t ghcr.io/mylab-tools/email-mgr:%1 -t ghcr.io/mylab-tools/email-mgr:latest ../src

echo "Publish image '%1' ..."
docker push ghcr.io/mylab-tools/email-mgr:%1

echo "Publish image 'latest' ..."
docker push ghcr.io/mylab-tools/email-mgr:latest

goto done

:noparam
echo "Please specify image version"
goto done

:done
echo "Done!"