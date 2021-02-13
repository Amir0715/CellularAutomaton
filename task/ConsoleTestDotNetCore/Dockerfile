FROM mcr.microsoft.com/dotnet/sdk:latest

ADD gRPCWorker/bin/Debug/net5.0 /opt/worker
WORKDIR /opt/worker

EXPOSE 5002

ENTRYPOINT ["bash"]
