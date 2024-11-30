/**
 * Declaración de módulos para archivos de imagen.
 * 
 * Estos módulos permiten importar archivos de imagen con extensiones .jpg y .png
 * como cadenas de texto en TypeScript, facilitando su uso en aplicaciones React.
 */
declare module '*.jpg' {
    const value: string;
    export default value;
}

declare module '*.png' {
    const value: string;
    export default value;
}