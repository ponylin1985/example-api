import cors from "cors";
import dotenv from "dotenv";
import express, { Request, Response } from "express";
import path from "path";
import "reflect-metadata";
import swaggerJsdoc from "swagger-jsdoc";
import swaggerUi from "swagger-ui-express";
import { errorHandler } from "./middlewares/globalErrorHandlerMiddleware";
import { requestLogger } from "./middlewares/requestResponseMiddleware";
import routes from "./routes";

dotenv.config({ path: path.resolve(__dirname, "../../../.env") });

const app = express();
app.use(cors());
app.use(express.json());
app.use(requestLogger);

const swaggerOptions = {
  definition: {
    openapi: "3.0.0",
    info: {
      title: "jjyy Example API (Node.js)",
      version: "1.0.0",
      description: "A Node.js implementation of the jjyy Example API",
    },
    servers: [
      {
        url: "http://localhost:5001",
      },
    ],
  },
  apis: ["./src/routes/*.ts", "./src/controllers/*.ts", "./src/dtos/*.ts"],
};

const specs = swaggerJsdoc(swaggerOptions);
app.use("/swagger", swaggerUi.serve, swaggerUi.setup(specs));

app.get("/healthz", (req: Request, res: Response) => {
  res.status(200).json({ status: "ok" });
});

app.use("/api", routes);
app.use(errorHandler);

export { app };
