FROM node:22-alpine AS build
WORKDIR /app

RUN npm install -g npm@11.6.1

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:1.27-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/tarefas-app/browser /usr/share/nginx/html

EXPOSE 80
