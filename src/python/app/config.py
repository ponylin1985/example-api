"""Configuration settings for the application."""

from pathlib import Path
from pydantic import field_validator
from pydantic_settings import BaseSettings


# Get the root directory of the git repo (../../ from this file)
ROOT_DIR = Path(__file__).parent.parent.parent.parent
ENV_FILE = ROOT_DIR / ".env"


class Settings(BaseSettings):
    """Application settings."""

    # PostgreSQL configuration
    postgres_user: str = "postgres"
    postgres_password: str = "postgres"

    # Server configuration
    server_host: str = "0.0.0.0"
    server_port: int = 5002

    # Logging configuration
    log_level: str = "INFO"

    @field_validator("log_level")
    @classmethod
    def uppercase_log_level(cls, v: str) -> str:
        """Convert log level to uppercase for Python logging compatibility."""
        return v.upper()

    @property
    def database_url(self) -> str:
        """Construct database URL from PostgreSQL settings."""
        return f"postgresql+asyncpg://{self.postgres_user}:{self.postgres_password}@localhost:5432/exampledb"

    class Config:
        """Pydantic configuration."""

        env_file = str(ENV_FILE)
        case_sensitive = False
        extra = "ignore"  # Ignore extra fields from .env file


settings = Settings()
