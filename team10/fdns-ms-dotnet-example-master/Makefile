docker-build:
	docker build \
		-t fdns-ms-dotnet-example \
		--rm \
		--no-cache \
		--build-arg ASPNETCORE_ENVIRONMENT=production \
		--build-arg APP_NAME=Example-Bookstore-Customer \
		--build-arg OBJECT_URL=http://fdns-ms-object:8083/api/1.0 \
		--build-arg STORAGE_URL=http://fdns-ms-storage:8082/api/1.0 \
		--build-arg RULES_URL=http://fdns-ms-rules:8086/api/1.0 \
		--build-arg INDEXING_URL=http://fdns-ms-indexing:8084/api/1.0 \
		.

docker-run: docker-start
docker-start:
	docker-compose up -d
	docker run -d \
		-p 9091:9091 \
		--network=fdns-ms-dotnet-example_default  \
		--name=fdns-ms-dotnet-example_main \
		fdns-ms-dotnet-example

docker-stop:
	docker stop fdns-ms-dotnet-example_main || true
	docker rm fdns-ms-dotnet-example_main || true
	docker-compose down

docker-restart:
	make docker-stop 2>/dev/null || true
	make docker-start