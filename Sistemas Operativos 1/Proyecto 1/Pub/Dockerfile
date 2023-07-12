FROM golang
WORKDIR /
COPY . .
ENV GOOGLE_APPLICATION_CREDENTIALS="./llave.json"
RUN go mod download
EXPOSE 4444
CMD ["go", "run", "pub.go"]