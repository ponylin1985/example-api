"""Sanitizer validator for XSS attack prevention."""

import re


class SanitizerValidator:
    """Validator to detect and prevent XSS attacks in string inputs."""

    # Common XSS patterns to detect
    XSS_PATTERNS = [
        r"<script[\s/>]",  # Script tags (including self-closing)
        r"</script>",  # Script closing tag
        r"javascript:",  # JavaScript protocol
        r"on\w+\s*=",  # Event handlers (onclick, onload, etc.)
        r"<iframe[\s/>]",  # Iframe tags
        r"<object[\s/>]",  # Object tags
        r"<embed[\s/>]",  # Embed tags
        r"<link[\s/>]",  # Link tags
        r"<meta[\s/>]",  # Meta tags
        r"<img[^>]*onerror",  # Image with onerror
        r"<svg[^>]*onload",  # SVG with onload
        r"eval\s*\(",  # eval function
        r"expression\s*\(",  # CSS expression
        r"vbscript:",  # VBScript protocol
        r"data:text/html",  # Data URI with HTML
        r"<\s*script",  # Script with whitespace variations
        r"<.*?on\w+\s*=",  # Any tag with event handlers
    ]

    @classmethod
    def contains_xss(cls, value: str) -> bool:
        """
        Check if the string contains potential XSS attack patterns.

        Args:
            value: String to check

        Returns:
            True if XSS pattern detected, False otherwise
        """
        if not value:
            return False

        # Convert to lowercase for case-insensitive matching
        value_lower = value.lower()

        # Check against all XSS patterns
        for pattern in cls.XSS_PATTERNS:
            if re.search(pattern, value_lower, re.IGNORECASE | re.DOTALL):
                return True

        return False

    @classmethod
    def validate(cls, value: str, field_name: str = "Field") -> str:
        """
        Validate string for XSS attacks and raise ValueError if detected.

        Args:
            value: String to validate
            field_name: Name of the field for error message

        Returns:
            Original value if valid

        Raises:
            ValueError: If XSS pattern is detected
        """
        if cls.contains_xss(value):
            raise ValueError(f"{field_name} contains potentially dangerous content (XSS detected)")
        return value
