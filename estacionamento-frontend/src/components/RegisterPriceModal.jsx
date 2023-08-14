import React from 'react';
import {Button, DatePicker, Form, Input, Modal, Switch} from 'antd';

// eslint-disable-next-line react/prop-types
const RegisterPriceModal = ({visible, onClose, onSubmit}) => {
    const [form] = Form.useForm();

    const handleSubmit = () => {
        form.validateFields().then((values) => {
            form.resetFields();
            onSubmit(values);
        });
    };

    return (
        <Modal title="Cadastro de Preços" open={visible} onCancel={onClose} footer={null}>
            <Form form={form} layout="vertical" onFinish={handleSubmit}>
                <Form.Item label="Data Inicial" name="startDate" rules={[{required: true}]}>
                    <DatePicker/>
                </Form.Item>
                <Form.Item label="Data Final" name="endDate" rules={[{required: true}]}>
                    <DatePicker/>
                </Form.Item>
                <Form.Item label="Valor da hora Inicial" name="startTimeValue" rules={[{required: true}]}>
                    <Input type="number" step="0.01"/>
                </Form.Item>
                <Form.Item label="Valor Adicional por Hora" name="additionalFeePerHour" rules={[{required: true}]}>
                    <Input type="number" step="0.01"/>
                </Form.Item>
                <Form.Item label="Ativo" name="isActive" valuePropName="checked">
                    <Switch/>
                </Form.Item>
                <Button type="primary" htmlType="submit">
                    Cadastrar
                </Button>
            </Form>
        </Modal>
    );
};

export default RegisterPriceModal;
