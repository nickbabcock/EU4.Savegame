FROM mono:5.2.0.215 as builder
COPY . /tmp/
WORKDIR /tmp
RUN mono .nuget/nuget.exe restore && msbuild EU4.Savegame.sln /p:Configuration=Release

FROM mono:5.2.0.215
COPY --from=builder /tmp/EU4.Stats.Web/server/bin/ /opt/eu4stats/bin
EXPOSE 8888
VOLUME /opt/eu4stats/games
VOLUME /tmp
CMD cd /opt/eu4stats/bin && mono EU4.Stats.Web.exe
