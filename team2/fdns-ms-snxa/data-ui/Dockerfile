# build stage
FROM node:latest as builder
WORKDIR /app
COPY package.json yarn.lock ./
RUN yarn
COPY . ./
RUN yarn build


# create image stage
FROM nginx:latest

MAINTAINER lrp4

COPY --from=builder app/build /usr/share/nginx/html
RUN chmod 777 -R /usr/share/nginx/html
EXPOSE 80
