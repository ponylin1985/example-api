import { app } from "./app";
import { AppDataSource } from "./data-source";

const PORT = process.env.NODE_EXPRESS_PORT || 5001;

AppDataSource.initialize()
  .then(() => {
    console.log("Data Source has been initialized!");
    app.listen(PORT, () => {
      console.log(`Server is running on port ${PORT}`);
      console.log(`Swagger docs available at http://localhost:${PORT}/swagger`);
    });
  })
  .catch((err) => {
    console.error("Error during Data Source initialization", err);
  });
