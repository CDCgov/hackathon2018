FROM debian:latest
MAINTAINER Team 2 Hackathon 2018

RUN apt-get -qq update && apt-get -qq -y install curl bzip2 \
    && curl -sSL https://repo.continuum.io/miniconda/Miniconda3-latest-Linux-x86_64.sh -o /tmp/miniconda.sh \
    && bash /tmp/miniconda.sh -bfp /usr/local \
    && rm -rf /tmp/miniconda.sh
RUN conda install -y -c conda-forge \
        python \
        osmnx=0.8* \
        geopandas \
        rtree \
        libgdal \
        flask \
        ncurses \
    # && conda update conda \
    && apt-get -qq -y remove curl bzip2 \
    && apt-get -qq -y autoremove \
    && apt-get autoclean \
    && rm -rf /var/lib/apt/lists/* /var/log/dpkg.log \
    && conda clean --all --yes

#VOLUME . /app

COPY . /app

WORKDIR /app
#RUN mkdir output

EXPOSE 5000
ENV PATH /opt/conda/bin:$PATH
ENTRYPOINT ["python"]
CMD ["app.py"]
