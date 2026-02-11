from sqlalchemy.types import TypeDecorator, SmallInteger


class IntEnumType(TypeDecorator):
    impl = SmallInteger

    cache_ok = True

    def __init__(self, enum_class):
        self.enum_class = enum_class
        super().__init__()

    def process_bind_param(self, value, dialect):
        if value is None:
            return None
        # allow passing either enum or int
        try:
            return int(value)
        except Exception:
            return int(self.enum_class(value))

    def process_result_value(self, value, dialect):
        if value is None:
            return None
        return self.enum_class(value)
