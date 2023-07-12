FROM rust

WORKDIR /app

COPY . .

ENV DATABASE_URL='mysql://root:1234@35.184.7.29/Proyecto1'

ENV DATABASE_URL_COSMOS='mongodb://sopes1-g24-2021:kxeCcSywgmVVNUgN2vuDMPKwULZ01ZryPyJQm3R8SjfJeG2WB3pBd7BmwI8pA3nnd28No0gJIUOBLnK5JoNWdw==@sopes1-g24-2021.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@sopes1-g24-2021@'

RUN rustup default nightly

RUN cargo install --path .

CMD ["API_Rust"]