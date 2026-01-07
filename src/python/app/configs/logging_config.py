"""Logging configuration for the application."""

import logging
import sys
import json
from logging.handlers import RotatingFileHandler
from pathlib import Path
from datetime import datetime
from app.configs.database_config import settings


class JSONFormatter(logging.Formatter):
    """Custom JSON formatter for structured logging."""

    def format(self, record: logging.LogRecord) -> str:
        """
        Format log record as JSON.

        Args:
            record: Log record to format

        Returns:
            JSON formatted log string
        """
        log_data = {
            "timestamp": datetime.utcfromtimestamp(record.created).isoformat() + "Z",
            "level": record.levelname,
            "logger": record.name,
            "message": record.getMessage(),
            "module": record.module,
            "filename": record.filename,
            "lineno": record.lineno,
            "funcName": record.funcName,
            "process": record.process,
            "thread": record.thread,
            "threadName": record.threadName,
        }

        if record.exc_info:
            log_data["exception"] = self.formatException(record.exc_info)

        if record.stack_info:
            log_data["stack_info"] = record.stack_info

        # Add any extra fields passed via logger.info(..., extra={...})
        # Filter out standard LogRecord attributes
        standard_attrs = {
            'name', 'msg', 'args', 'created', 'filename', 'funcName', 'levelname', 
            'levelno', 'lineno', 'module', 'msecs', 'pathname', 'process', 
            'processName', 'relativeCreated', 'thread', 'threadName', 'exc_info', 
            'exc_text', 'stack_info', 'getMessage', 'message'
        }
        for key, value in record.__dict__.items():
            if key not in standard_attrs and not key.startswith('_'):
                log_data[key] = value

        return json.dumps(log_data, ensure_ascii=False)


def setup_logging():
    """
    Configure logging for the application.
    Sets up console (JSON format) and file handlers (text format) with rotating file logs.
    """
    # Create logs directory if it doesn't exist
    log_dir = Path(__file__).parent.parent.parent / "logs"
    log_dir.mkdir(exist_ok=True)

    log_file = log_dir / "app.log"
    error_log_file = log_dir / "error.log"

    # Create formatters
    json_formatter = JSONFormatter()

    detailed_formatter = logging.Formatter(
        fmt="%(asctime)s - %(name)s - %(levelname)s - [%(filename)s:%(lineno)d] - %(message)s",
        datefmt="%Y-%m-%d %H:%M:%S",
    )

    simple_formatter = logging.Formatter(fmt="%(asctime)s - %(levelname)s - %(message)s", datefmt="%Y-%m-%d %H:%M:%S")

    # Configure root logger
    root_logger = logging.getLogger()
    root_logger.setLevel(settings.log_level)

    # Remove existing handlers to avoid duplicates
    root_logger.handlers.clear()

    # Console handler - JSON format for container logs
    console_handler = logging.StreamHandler(sys.stdout)
    console_handler.setLevel(settings.log_level)
    console_handler.setFormatter(json_formatter)
    root_logger.addHandler(console_handler)

    # File handler - for all logs (rotating, max 10MB, keep 5 backups)
    file_handler = RotatingFileHandler(log_file, maxBytes=10 * 1024 * 1024, backupCount=5, encoding="utf-8")  # 10MB
    file_handler.setLevel(settings.log_level)
    file_handler.setFormatter(detailed_formatter)
    root_logger.addHandler(file_handler)

    # Error file handler - only for ERROR and CRITICAL (rotating, max 10MB, keep 5 backups)
    error_file_handler = RotatingFileHandler(
        error_log_file, maxBytes=10 * 1024 * 1024, backupCount=5, encoding="utf-8"  # 10MB
    )
    error_file_handler.setLevel(logging.ERROR)
    error_file_handler.setFormatter(detailed_formatter)
    root_logger.addHandler(error_file_handler)

    # Log startup message
    logger = logging.getLogger(__name__)
    logger.info("Logging configured - Log level: %s", settings.log_level)
    logger.info("Log files: %s, %s", log_file, error_log_file)

    return root_logger
