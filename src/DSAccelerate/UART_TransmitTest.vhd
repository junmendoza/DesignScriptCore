----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    21:45:46 01/30/2015 
-- Design Name: 
-- Module Name:    UART_TransmitTest - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity UART_TransmitTest is
	Port( 
			clock : in  STD_LOGIC;
			reset : in  STD_LOGIC;
			RS232_dataout : out STD_LOGIC;
			LED : out STD_LOGIC_VECTOR(7 downto 0)
		 );
			  
end UART_TransmitTest;

architecture Behavioral of UART_TransmitTest is

	component BaudRateGenerator is
		Port( 
				clock : in STD_LOGIC;
				reset : in STD_LOGIC;
				baudRateEnable : out STD_LOGIC
			 );
	end component BaudRateGenerator;

	component UART_Transmitter is
		Port( 
				clock : in STD_LOGIC;
				reset : in STD_LOGIC;
				transmit : in STD_LOGIC;
				send_data : in STD_LOGIC_VECTOR(7 downto 0);
				dataout : out STD_LOGIC;
				done : out STD_LOGIC 
			 );
	end component UART_Transmitter;

	signal baudRateEnable : STD_LOGIC := '0';
	signal transmit_byte_done : STD_LOGIC := '0';
	signal serial_data : STD_LOGIC_VECTOR(31 downto 0) := X"CCCCCCCC";
	signal start_send_byte : STD_LOGIC := '0';
	signal byte_to_send : STD_LOGIC_VECTOR(7 downto 0);
	signal transmit_done : STD_LOGIC := '0';

begin

	baudrate : BaudRateGenerator port map
	(
		clock => clock,
		reset => reset,
		baudRateEnable => baudRateEnable
	);
	
	uart_transmitdata : UART_Transmitter port map
	(
		clock => clock,
		reset => reset,
		transmit => start_send_byte,
		send_data => byte_to_send,
		dataout => RS232_dataout,
		done => transmit_byte_done
	);

	tx_send_byte : process(baudRateEnable)
		variable lo_bits : integer;
		variable hi_bits : integer;
	begin
		if reset = '1' then
			lo_bits := 0;
			hi_bits := 7;
			start_send_byte <= '0';
			transmit_done <= '0';
		elsif reset = '0' then 
			if transmit_done = '0' then
				if baudRateEnable = '1' then
					if lo_bits = 32 then
						transmit_done <= '1';
					else
						start_send_byte <= '1';
						byte_to_send(7 downto 0) <= serial_data(hi_bits downto lo_bits);
						lo_bits := lo_bits + 8;
						hi_bits := hi_bits + 8;
					end if;
				elsif baudRateEnable = '0' then
					start_send_byte <= '0';
				end if;
			end if;
		end if;
	end process tx_send_byte;
	
	tx_complete : process(reset, transmit_done)
	begin
		if reset = '1' then
			LED(0) <= '0';
			LED(1) <= '0';
			LED(2) <= '0';
			LED(3) <= '0';
			LED(4) <= '0';
			LED(5) <= '0';
			LED(6) <= '0';
			LED(7) <= '0';
		elsif reset = '0' then 
			if transmit_done = '1' then
				LED(0) <= '1';
			end if;
		end if;
	end process tx_complete;

end Behavioral;

