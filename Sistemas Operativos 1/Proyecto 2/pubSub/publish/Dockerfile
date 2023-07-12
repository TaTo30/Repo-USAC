FROM golang
WORKDIR /
COPY . .
RUN go mod download
ENV PORT 3333
EXPOSE 3333
CMD ["go", "run", "pub.go"]