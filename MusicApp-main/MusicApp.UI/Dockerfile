# FROM node:lts-alpine
# ENV NODE_ENV=production
# WORKDIR /usr/src/app
# COPY ["package.json", "package-lock.json*", "npm-shrinkwrap.json*", "./"]
# RUN npm install --production --silent && mv node_modules ../
# COPY . .
# EXPOSE 3000
# RUN chown -R node /usr/src/app
# USER node
# CMD ["npm", "start"]


# Step 1: Build
FROM node:20-alpine as build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# Step 2: Serve using nginx
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80