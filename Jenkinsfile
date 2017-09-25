pipeline {
  agent {
    docker {
      image 'mono:5.2.0.215'
    }
    
  }
  stages {
    stage('Build') {
      steps {
        sh '''mono .nuget/nuget.exe restore;
msbuild EU4.Savegame.sln /p:Configuration=Release'''
      }
    }
  }
}