FROM golang:1.15

WORKDIR /app

COPY . .

RUN go build

ENV PORT 8000

ENV IP "192.168.0.0"

CMD ["./app"]

