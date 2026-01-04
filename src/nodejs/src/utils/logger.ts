import winston from "winston";
import path from "path";

const logLevel = process.env.LOG_LEVEL || "info";
const logDir = "logs";

const formats = [
  winston.format.timestamp({ format: "YYYY-MM-DD HH:mm:ss.SSS" }),
  winston.format.errors({ stack: true }),
  winston.format.splat(),
  winston.format.json(),
];

const logger = winston.createLogger({
  level: logLevel,
  format: winston.format.combine(...formats),
  transports: [
    //
    // - Write all logs with importance level of `error` or less to `error.log`
    // - Write all logs with importance level of `info` or less to `combined.log`
    //
    new winston.transports.File({ 
        filename: path.join(logDir, "error.log"), 
        level: "error",
        maxsize: 5242880, // 5MB
        maxFiles: 5,
    }),
    new winston.transports.File({ 
        filename: path.join(logDir, "combined.log"),
        maxsize: 5242880, // 5MB
        maxFiles: 5,
    }),
  ],
});

//
// If we're not in production then log to the `console` with the format:
// `${info.level}: ${info.message} JSON.stringify({ ...rest }) `
//
if (process.env.NODE_ENV !== "production") {
  logger.add(
    new winston.transports.Console({
      format: winston.format.combine(
        winston.format.colorize(),
        winston.format.printf(({ timestamp, level, message, stack, ...meta }) => {
            return `${timestamp} [${level}]: ${message} ${stack ? '\n' + stack : ''} ${Object.keys(meta).length ? JSON.stringify(meta) : ''}`;
        })
      ),
    })
  );
}

export default logger;
