import { Bar } from "react-chartjs-2";
import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    BarElement,
    Title,
    Tooltip,
    Legend,
    Filler
} from 'chart.js';
import { PlotProps } from "../../interfaces/PlotProps";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    BarElement,
    Title,
    Tooltip,
    Legend,
    Filler
)

export function BarsChart({ labels, data, label }: PlotProps) {
    const myData = {
        labels: labels,
        datasets: [
            {
                label: label,
                data: data,
                backgroundColor: 'rgba(0, 220, 195, 0.5)',
            },
        ],
    };

    const misOptions = {};

    return <Bar data={myData} options={misOptions} />
}