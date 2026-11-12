pipeline {
    agent any

    stages {
        stage('CI') {
            when {
                changeRequest target: 'main'
            }
            steps {
                echo 'Đang chạy CI cho PR vào main'
                // Thay bằng lệnh build/test thực tế của dự án
            }
        }
    }
}
