FROM golang
WORKDIR /
COPY . .
RUN go mod download
ENV PORT 4444
EXPOSE 4444
CMD ["go", "run", "faultyTraffic.go"]