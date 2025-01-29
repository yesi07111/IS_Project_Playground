import React from "react";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import { Pie } from 'react-chartjs-2';
import { PlotProps } from "../../interfaces/PlotProps";

ChartJS.register(ArcElement, Tooltip, Legend);

// Función para generar colores aleatorios
const generateColors = (length: number) => {
    const colors = [];
    for (let i = 0; i < length; i++) {
        // Generar colores aleatorios con el formato rgba
        const r = Math.floor(Math.random() * 256);
        const g = Math.floor(Math.random() * 256);
        const b = Math.floor(Math.random() * 256);
        colors.push(`rgba(${r}, ${g}, ${b}, 0.5)`);
    }
    return colors;
};

export default function PiesChart({ labels, data, label }: PlotProps) {
    // Generamos los colores basados en la longitud del array 'data' o 'labels'
    const backgroundColors = generateColors(data.length);

    const options = {
        responsive: true,
        maintainAspectRatio: false,
    };

    const myData = {
        labels: labels,
        datasets: [
            {
                label: label,
                data: data,
                backgroundColor: backgroundColors,
                borderColor: backgroundColors.map((color) => color.replace('0.5', '1')),  // Usamos el mismo color con opacidad completa para el borde
                borderWidth: 1,
            },
        ],
    };

    return <Pie data={myData} options={options} />
}
