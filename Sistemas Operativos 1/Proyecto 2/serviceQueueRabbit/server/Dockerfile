FROM golang
WORKDIR /
COPY . .
RUN go mod download
ENV PORT 9080
EXPOSE 9080
CMD ["go", "run", "server.go"]