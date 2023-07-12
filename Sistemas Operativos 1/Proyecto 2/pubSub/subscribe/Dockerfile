FROM golang
WORKDIR /
COPY . .
RUN go mod download
ENV PORT 80
EXPOSE 80
CMD ["go", "run", "sub.go"]