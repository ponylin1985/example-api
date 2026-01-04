import "reflect-metadata";
import express, { Request, Response, NextFunction } from "express";
import cors from "cors";
import swaggerUi from "swagger-ui-express";
import swaggerJsdoc from "swagger-jsdoc";
import { AppDataSource } from "./data-source";
import routes from "./routes";
import { errorHandler } from "./middlewares/errorHandler";
import { requestLogger } from "./middlewares/requestLogger";

const app = express();

app.use(cors());
app.use(express.json());
app.use(requestLogger);

// Swagger Setup
const options = {
  definition: {
    openapi: "3.0.0",
    info: {
      title: "Jubo Example API (Node.js)",
      version: "1.0.0",
      description: "A Node.js implementation of the Jubo Example API",
    },
    servers: [
      {
        url: "http://localhost:5000",
      },
    ],
  },
  apis: ["./src/routes/*.ts", "./src/controllers/*.ts", "./src/dtos/*.ts"],
};

const specs = swaggerJsdoc(options);
app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(specs));

// Health Check
app.get("/health", (req: Request, res: Response) => {
  res.status(200).json({ status: "ok" });
});

// Routes
app.use("/api", routes);

// Global Error Handler
app.use(errorHandler);

export { app };
