import { FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { DaySelectorProps } from "../../interfaces/Filters";

export const DaySelector: React.FC<DaySelectorProps> = ({ daysOfWeek, setDaysOfWeek }) => {
    const currentDay = new Date().getDay(); // Obtiene el día actual (0 = Domingo, 1 = Lunes, ..., 6 = Sábado)

    // Ajusta el índice para que el lunes sea el primer día de la semana
    const adjustedCurrentDay = (currentDay === 0) ? 6 : currentDay - 1;

    const getDayLabel = (day: number): string => {
        const dayNames = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];
        const emojis = ['🌕', '🔥', '🌳', '⚡', '🌊', '🪐', '🌞']; // Emojis representativos para cada día

        // Ajusta el índice del día para que coincida con el nuevo orden
        const adjustedDay = (day === 0) ? 6 : day - 1;

        const dayLabel = adjustedDay < adjustedCurrentDay ? `Prox. ${dayNames[adjustedDay]}` : dayNames[adjustedDay];
        return `${dayLabel} ${emojis[adjustedDay]}`;
    };

    return (
        <FormControl sx={{ width: '300px', height: '56px', mr: 2 }} variant="outlined">
            <InputLabel id="dias-especificos-label">📝 Escoger Días</InputLabel>
            <Select
                labelId="dias-especificos-label"
                label="Días específicos"
                multiple
                value={daysOfWeek}
                onChange={(e) => setDaysOfWeek(e.target.value as string[])}
                renderValue={(selected) => selected.map(day => getDayLabel(parseInt(day))).join(', ')}
                sx={{
                    height: '56px',
                    '& .MuiSelect-select': {
                        paddingTop: '14px',
                        paddingBottom: '14px',
                    },
                }}
            >
                {[1, 2, 3, 4, 5, 6, 0].map(day => (
                    <MenuItem key={day} value={day.toString()}>
                        {getDayLabel(day)}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
};