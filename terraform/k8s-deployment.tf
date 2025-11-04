terraform {
  required_providers {
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.0"
    }
  }
}

provider "kubernetes" {
  config_path = "~/.kube/config"
}

resource "kubernetes_namespace" "dating_app" {
  metadata {
    name = "dating-app"
  }
}

resource "kubernetes_deployment" "userservice" {
  metadata {
    name      = "userservice"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "userservice"
      }
    }
    template {
      metadata {
        labels = {
          app = "userservice"
        }
      }
      spec {
        container {
          name  = "userservice"
          image = "harifosa/dating-userservice:latest"
          port {
            container_port = 8080
          }
          env {
            name  = "MONGODB_CONNECTION_STRING"
            value = "mongodb://admin:password123@mongodb:27017/datingapp?authSource=admin"
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "userservice" {
  metadata {
    name      = "userservice"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    selector = {
      app = "userservice"
    }
    port {
      port        = 8080
      target_port = 8080
    }
    type = "ClusterIP"
  }
}

resource "kubernetes_deployment" "photoservice" {
  metadata {
    name      = "photoservice"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "photoservice"
      }
    }
    template {
      metadata {
        labels = {
          app = "photoservice"
        }
      }
      spec {
        container {
          name  = "photoservice"
          image = "harifosa/dating-photoservice:latest"
          port {
            container_port = 8080
          }
          volume_mount {
            name       = "photo-storage"
            mount_path = "/app/wwwroot"
          }
        }
        volume {
          name = "photo-storage"
          empty_dir {}
        }
      }
    }
  }
}

resource "kubernetes_service" "photoservice" {
  metadata {
    name      = "photoservice"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    selector = {
      app = "photoservice"
    }
    port {
      port        = 8080
      target_port = 8080
    }
    type = "ClusterIP"
  }
}

resource "kubernetes_deployment" "matchservice" {
  metadata {
    name      = "matchservice"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "matchservice"
      }
    }
    template {
      metadata {
        labels = {
          app = "matchservice"
        }
      }
      spec {
        container {
          name  = "matchservice"
          image = "harifosa/dating-matchservice:latest"
          port {
            container_port = 8080
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "matchservice" {
  metadata {
    name      = "matchservice"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    selector = {
      app = "matchservice"
    }
    port {
      port        = 8080
      target_port = 8080
    }
    type = "ClusterIP"
  }
}

resource "kubernetes_stateful_set" "mongodb" {
  metadata {
    name      = "mongodb"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    service_name = "mongodb"
    replicas     = 1
    selector {
      match_labels = {
        app = "mongodb"
      }
    }
    template {
      metadata {
        labels = {
          app = "mongodb"
        }
      }
      spec {
        container {
          name  = "mongodb"
          image = "mongo:7.0"
          port {
            container_port = 27017
          }
          env {
            name  = "MONGO_INITDB_ROOT_USERNAME"
            value = "admin"
          }
          env {
            name  = "MONGO_INITDB_ROOT_PASSWORD"
            value = "password123"
          }
          volume_mount {
            name       = "mongodb-data"
            mount_path = "/data/db"
          }
          resources {
            requests = {
              memory = "256Mi"
              cpu    = "200m"
            }
            limits = {
              memory = "512Mi"
              cpu    = "500m"
            }
          }
        }
        volume {
          name = "mongodb-data"
          empty_dir {}
        }
      }
    }
  }
}

resource "kubernetes_service" "mongodb" {
  metadata {
    name      = "mongodb"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    selector = {
      app = "mongodb"
    }
    port {
      port        = 27017
      target_port = 27017
    }
    type = "ClusterIP"
  }
}

resource "kubernetes_deployment" "frontend" {
  metadata {
    name      = "frontend"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "frontend"
      }
    }
    template {
      metadata {
        labels = {
          app = "frontend"
        }
      }
      spec {
        container {
          name  = "frontend"
          image = "harifosa/dating-frontend:latest"
          port {
            container_port = 3000
          }
          env {
            name  = "REACT_APP_API_BASE_URL"
            value = "http://a9df033cda67344e9a58c7ef6b9ed3f3-772509191.us-east-1.elb.amazonaws.com"
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "frontend" {
  metadata {
    name      = "frontend"
    namespace = kubernetes_namespace.dating_app.metadata[0].name
  }
  spec {
    selector = {
      app = "frontend"
    }
    port {
      port        = 80
      target_port = 3000
    }
    type = "LoadBalancer"
  }
}