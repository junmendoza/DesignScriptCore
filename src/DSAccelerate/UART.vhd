----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    10:29:42 02/08/2015 
-- Design Name: 
-- Module Name:    UART - Behavioral 
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

entity UART is
	Port( 
			clock  : in  STD_LOGIC;
			reset : in  STD_LOGIC;	
			start_transmit : in STD_LOGIC;
			send_byte : in STD_LOGIC_VECTOR(7 downto 0);
			dataout : out STD_LOGIC;
			byte_sent : out STD_LOGIC 
		 );
end UART;

architecture Behavioral of UART is

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
				baudRateEnable : in STD_LOGIC;
				send_data : in STD_LOGIC_VECTOR(7 downto 0);
				dataout : out STD_LOGIC;
				done : out STD_LOGIC 
			 );
	end component UART_Transmitter;

	signal baudRateEnable : STD_LOGIC := '0';
	
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
		transmit => start_transmit,
		baudRateEnable => baudRateEnable,
		send_data => send_byte,
		dataout => dataout,
		done => byte_sent
	);

end Behavioral;

