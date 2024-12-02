import React from 'react';
import { Controller } from 'react-hook-form';
import { Button, Box, Typography, TextField } from '@mui/material';

const FileUploadField = ({ name, control, label, rules = {} }) => {
    return (
        <Controller
            name={name}
            control={control}
            rules={rules}
            render={({ field: { onChange }, fieldState: { error } }) => (
                <Box sx={{ marginBottom: 2 }}>
                    <Typography variant="body1" gutterBottom>
                        {label}
                    </Typography>
                    <Button
                        variant="contained"
                        component="label"
                        sx={{ marginRight: 2 }}
                    >
                        Upload File
                        <input
                            type="file"
                            hidden
                            onChange={(e) => {
                                const file = e.target.files[0];
                                onChange(file);
                            }}
                        />
                    </Button>
                    {error && (
                        <Typography variant="caption" color="error">
                            {error.message}
                        </Typography>
                    )}
                </Box>
            )}
        />
    );
};

export default FileUploadField;
