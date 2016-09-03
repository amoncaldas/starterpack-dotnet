require('package-script').spawn([
  {
    command: "npm",
    args: ["install", "-g", "yo"],
    admin: true
  },
  {
    command: "npm",
    args: ["install", "-g", "generator:prgenerator"],
    admin: true
  }
]);
