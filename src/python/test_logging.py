"""測試錯誤日誌功能的腳本"""

import logging
from app.configs.logging_config import setup_logging

setup_logging()
logger = logging.getLogger(__name__)


def test_logging():
    """測試各種級別的日誌"""
    logger.debug("This is a DEBUG message")
    logger.info("This is an INFO message")
    logger.warning("This is a WARNING message")
    logger.error("This is an ERROR message")

    try:
        result = 1 / 0
    except ZeroDivisionError as e:
        logger.error("Division by zero error occurred", exc_info=True)

    logger.critical("This is a CRITICAL message")

    print("\n✓ Log files should be created in src/python/logs/ directory:")
    print("  - app.log (all logs)")
    print("  - error.log (errors and critical only)")


if __name__ == "__main__":
    test_logging()
