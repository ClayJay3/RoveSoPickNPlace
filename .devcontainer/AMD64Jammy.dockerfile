FROM ubuntu:jammy-20240627.1

# Set Non-Interactive Mode
ARG DEBIAN_FRONTEND=noninteractive

# Set Timezone
ARG TZ="America/Chicago"
RUN echo "${TZ}" > /etc/localtime && \
    echo "${TZ}" > /etc/timezone

# 1) Install all your Ubuntu packages (including Ruby)
RUN apt-get update && \
    apt-get install --no-install-recommends -y \
      python3 \
      python-is-python3 \
      python3-msgpack \
      gnome-keyring \
      sqlite3 \
      nano \
      net-tools \
      git \
      ssh \
      git-lfs \
      libexpat1 \
      apt-transport-https \
      software-properties-common \
      wget \
      unzip \
      ca-certificates \
      build-essential \
      cmake \
      libtbb-dev \
      libatlas-base-dev \
      libgtk2.0-dev \
      libavcodec-dev \
      libavformat-dev \
      libswscale-dev \
      libdc1394-dev \
      libxine2-dev \
      libv4l-dev \
      libtheora-dev \
      libvorbis-dev \
      libxvidcore-dev \
      libopencore-amrnb-dev \
      libopencore-amrwb-dev \
      x264 \
      libtesseract-dev \
      libgdiplus \
      btop \
      bash-completion \
      fish \
      curl \
      nuget \
      libpq-dev libpcap-dev \
      ruby-full

# 2) Install Bundler via gem
RUN gem install bundler

# 3) Clean up apt cache
RUN rm -rf /var/lib/apt/lists/*

# Install .NET SDK
RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --channel LTS --install-dir /usr/share/dotnet && \
    ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet && \
    rm dotnet-install.sh

# Install Metasploit
RUN curl https://raw.githubusercontent.com/rapid7/metasploit-omnibus/master/config/templates/metasploit-framework-wrappers/msfupdate.erb \
      -o msfinstall && \
    chmod +x msfinstall && \
    ./msfinstall && \
    rm msfinstall

# Switch default shell to fish and set a minimal greeting
RUN chsh -s /usr/bin/fish && \
    mkdir -p /root/.config/fish/ && \
    echo 'set fish_greeting' >> /root/.config/fish/config.fish

# Set Labels
LABEL authors="ClayJay3 (claytonraycowen@gmail.com)"
LABEL maintainer="Craysoftware"
LABEL org.opencontainers.image.source="https://github.com/clayjay3/RoveSoPickNPlace"
LABEL org.opencontainers.image.licenses="GPL-3.0-only"
LABEL org.opencontainers.image.version="v0.0.1"
LABEL org.opencontainers.image.description="Docker Image for Ubuntu Blazor Server Development"
