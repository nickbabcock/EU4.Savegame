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
    stage('Test') {
      steps {
        sh '(cd EU4.Savegame.Test/bin && mono ../../packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe EU4.Savegame.Test.dll)'
        junit 'EU4.Savegame.Test/bin/TestResult.xml'
      }
    }
  }
}