import React from 'react';
import { TextField } from '@mui/material';
import { Controller } from 'react-hook-form';

const ReusableTextBox = ({
    name,
    label,
    control,
    rules = {},
    defaultValue = '',
    type = 'text',
    ...rest
}) => {
    return (
        <Controller
            name={name}
            control={control}
            defaultValue={defaultValue}
            rules={rules}
            render={({ field, fieldState: { error } }) => (
                <TextField
                    {...field}
                    label={label}
                    type={type}
                    fullWidth
                    variant="outlined"
                    margin="normal"
                    error={!!error}
                    helperText={error ? error.message : ''}
                    {...rest}
                />
            )}
        />
    );
};

export default ReusableTextBox;
