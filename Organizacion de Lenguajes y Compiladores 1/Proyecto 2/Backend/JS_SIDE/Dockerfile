FROM node:14.12

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

CMD [ "npm", "start"]