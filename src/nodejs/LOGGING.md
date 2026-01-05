# Request Response Logging Configuration

This document describes how to configure request and response logging for the Node.js API.

## Environment Variables

The following environment variables control the logging behavior:

- `LOG_LEVEL`: Sets the logging level (e.g., `debug`, `info`, `warn`, `error`). Default: `info`
- `LOG_REQUEST_BODY`: Enable request body logging. Set to `true` to enable. Default: `false`
- `LOG_RESPONSE_BODY`: Enable response body logging. Set to `true` to enable. Default: `false`

## Configuration Example

Add these variables to your `.env` file:

```env
LOG_LEVEL=debug
LOG_REQUEST_BODY=true
LOG_RESPONSE_BODY=true
```

## Usage

### Default Usage

The default `requestLogger` middleware is automatically configured using environment variables:

```typescript
import { requestLogger } from "./middlewares/requestLogger";

app.use(requestLogger);
```

### Custom Configuration

You can also create a custom logger with specific options:

```typescript
import { createRequestLogger } from "./middlewares/requestLogger";

const customLogger = createRequestLogger({
  enabledRequestLog: true,
  enabledResponseLog: false,
});

app.use(customLogger);
```

## Log Levels

- **info**: Logs basic request information (method, URL, status code, duration)
- **warn**: Logs requests that resulted in 4xx or 5xx status codes
- **debug**: Logs detailed request/response information including request body, response body, query strings, etc.

## Notes

- Request/response body logging is only active when `LOG_LEVEL=debug` and the respective environment variables are set to `true`
- Large request/response bodies may impact performance and log storage
- Sensitive data in request/response bodies will be logged - use with caution in production
