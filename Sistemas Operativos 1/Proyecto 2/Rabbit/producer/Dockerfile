FROM golang

WORKDIR /

COPY . .

RUN go mod download

ENV PORT 4000

EXPOSE 4000

CMD [ "go", "run", "main.go" ]
