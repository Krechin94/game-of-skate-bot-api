FROM ngrok/ngrok:debian as main

ARG TUNNEL_TOKEN
ARG TUNNEL_EDGE
ARG TUNNEL_HOST
ENV TUNNEL_TOKEN="${TUNNEL_TOKEN}"
ENV TUNNEL_EDGE="${TUNNEL_EDGE}"
ENV TUNNEL_HOST="${TUNNEL_HOST}"

USER root
RUN apt-get update \
    && apt-get install -y \
        gettext \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*
COPY ngrok.yml /etc/ngrok.yml
RUN sh -c 'envsubst < /etc/ngrok.yml > /etc/ngrok.yml.tmp \
    && mv /etc/ngrok.yml.tmp /etc/ngrok.yml'
USER 1001
ENTRYPOINT [ "ngrok" , "start", "--all", "--config", "/etc/ngrok.yml" ]