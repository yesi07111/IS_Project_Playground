declare module '@ahl389/react-rainbowify' {
    import { ComponentType, ReactNode } from 'react';
    import { TypographyProps } from '@mui/material/Typography';

    interface RainbowifyProps extends TypographyProps {
        component: ComponentType<TypographyProps>;
        children?: ReactNode;
    }

    const Rainbowify: ComponentType<RainbowifyProps>;

    export default Rainbowify;
}